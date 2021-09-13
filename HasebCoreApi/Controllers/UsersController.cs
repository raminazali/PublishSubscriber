using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using HasebCoreApi.DTO;
using HasebCoreApi.DTO.Common;
using HasebCoreApi.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using HasebCoreApi.Localize;
using HasebCoreApi.Helpers;

namespace HasebCoreApi.Controllers
{
    [ApiController]
    [Route("api/core/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IServiceWrapper _serviceWrapper;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer _localizer;
        public UsersController(IServiceWrapper serviceWrapper, IMapper mapper, IStringLocalizer<Resource> localizer)
        {
            _mapper = mapper;
            _serviceWrapper = serviceWrapper;
            _localizer = localizer;
        }

        /// <summary>
        /// Authenticate an user.If user authenticate successfully, sending a otp(sms) to him/her.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "Username": "ali",
        ///        "Password": "123",
        ///        "Mobile" : "9655236532"  => Needed if user authenticated on digi maliat and has no mobile
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Authentication is successfull and sms sent to user mobile</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 0, Message = "Username or password is wrong." }
        ///     
        ///     { Code = 1, Message = "Your account is inactive." } 
        ///     
        ///     { Code = 2, Message = "You must wait for 00:02:01.65457", Data = "00:02:01.65457" } 
        ///     
        ///     { Code = 3, Message = "You must enter your mobile also" } 
        ///     
        ///     { Code = 4, Message = "An error occured in digi maliat mobile update", Data = "Error code" }
        ///     
        /// </response>
       
        [HttpPost("Authenticate")]
        public async Task<ActionResult<UserAuthSuccDTO>> Authenticate(UserAuthDTO user)
        {
            try
            {
                var (_user, isActive) = await _serviceWrapper.User.Authenticate(user.Username, user.Password);
                return Ok(new UserAuthSuccDTO { Mobile = _user.Mobile, IsActive = isActive, Username = _user.UserName, ServerDateTime = DateTime.Now });

            }
            catch (UserNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_user_notFound") });
            }
            catch (UserInActiveException)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("err_user_inActive") });
            }
            catch (UserSentOTPException ex)
            {
                var output = ex.RemainTime.ToString(@"hh\:mm\:ss");
                return BadRequest(new GenericMessage { Code = 2, Message = string.Format(_localizer.GetString("err_user_otp_sent"), output), Data = ex.RemainTime });
            }
            catch (UserHasNoMobileInDigiException)
            {
                return BadRequest(new GenericMessage { Code = 3, Message = _localizer.GetString("err_user_auth_mobile") });
            }
            catch (UserUpdateMobileInDigiException ex)
            {
                return BadRequest(new GenericMessage { Code = 4, Message = _localizer.GetString("err_user_auth_mobile_update"), Data = ex.DigiCode });
            }
            catch (UserMobileRepetitiousException)
            {
                return BadRequest(new GenericMessage { Code = 5, Message = _localizer.GetString("err_user_mobile_fix")});
            }
        }

        /// <summary> If user changes or sets him/his email,user must verify him/his email via link that has been sent to email address</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Description => Send Jwt-token In Request Header(Authrization) 
        ///     {
        ///         EmailOTP:"12345"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Email Verify Is Successfully!</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 0, Message = "Username or password is wrong" }
        ///     
        ///     { Code = 1, Message = "Username or password is wrong" }
        /// </response>
        [Authorize]
        [HttpPost("VerifyEmail")]
        public async Task<ActionResult<UserInfoDTO>> VerifyEmail(UserEmailOTPDTO userEmailOTPDTO)
        {
            try
            {
                await _serviceWrapper.User.VerifyEmail(userEmailOTPDTO.EmailOTP, User.GetUserId());
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_user_notFound") });
            }
            catch (InvalidOTPException)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("err_user_otp") });
            }
        }

        /// <summary> Change user email and send an OTP(token) to user email address to verify user's email</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Description => Send Jwt-token In Request Header(Authrization) 
        ///     {
        ///        Email: "Example@gmail.com"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Email For Verification Will be Send To You</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 0, Message = "User Not Exists!" }
        /// </response>
        [Authorize]
        [HttpPut("ChangeEmail")]
        public async Task<ActionResult> ChangeEmail(UserEmailDTO email)
        {
            try
            {
                await _serviceWrapper.User.ChangeEmail(email.Email, User.GetUserId());
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_user_notFound") });
            }

        }

        ///// <summary> Verify OTP that has been sent to user mobile after login or signup</summary>
        ///// <remarks>
        ///// Sample verify:
        /////
        /////     POST/Verify
        /////     {
        /////        "Username": "ramin",
        /////        "OTP": "98595" => Its the code that You will get from SMS, 5 Random Numbers, Like 98595
        /////        "Signup": true => if True means we want verify after Signup ||||| => if false we want verify after Authenticate
        /////        "ForgetPassword": true => It Should be True When You Want To Use ForgetPassword
        /////        "IsActive": true if Your in Digimaliat is inActive And You Want Change it Active
        /////     }
        ///

        /// </remarks>
        /// <response code="200">Verify Successfully And Token Will be Generate For You</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 0, Message = "Username or password is wrong." }
        ///     
        ///     { Code = 1, Message = "Invalid OTP or expired." } 
        ///     
        ///     { Code = 2, Message = "Your mobile is not validated in DG." } 
        ///     
        ///     { Code = 3, Message = "User is not active in haseb" } 
        /// </response>
        [HttpPost("Verify")]
        public async Task<ActionResult<UserInfoDTO>> Verify(UserOTPDTO userOTPDTO)
        {
            try
            {
                var (token, user) = await _serviceWrapper.User.Verify(userOTPDTO.Username, userOTPDTO.OTP, userOTPDTO.Signup, userOTPDTO.ForgetPassword, userOTPDTO.IsActive);

                var userInfoDTO = _mapper.Map<UserInfoDTO>(user);
                userInfoDTO.Token = token;
                userInfoDTO.ServerDateTime = DateTime.Now;

                return Ok(userInfoDTO);
            }
            catch (UserNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_user_notFound") });
            }
            catch (InvalidOTPException)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("err_user_otp") });
            }
            catch (UserValidateMobileInDigiException ex)
            {
                return BadRequest(new GenericMessage { Code = 2, Message = _localizer.GetString("err_user_verify_mobile_update"), Data = ex.DigiCode });
            }
            catch (UserInActiveException)
            {
                return BadRequest(new GenericMessage { Code = 3, Message = _localizer.GetString("err_user_inActive") });
            }
        }

        /// <summary> Send an OTP to user mobile for change password </summary>
        /// <remarks>
        /// Sample Request:
        ///
        ///     {
        ///         Identifier: "raminazali9" => it can be username OR mobile number OR Email Address
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Forget Password</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     {Code =0 , Message = "No User Found With This Email,Username Or Mobile Number" }
        /// </response>
        [HttpPost("ForgetPassword")]
        public async Task<ActionResult> ForgetPassword(UserForgetPasswordDTO userForgetPasswordDTO)
        {
            try
            {
                var user = await _serviceWrapper.User.ForgetPassword(userForgetPasswordDTO.Identifier);
                // send back Mobile number for Front
                return Ok(new ForgetPasswordMobileDTO { Mobile = user.Mobile });
            }
            catch (UserNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_user_notFound") });
            }
            catch (UserSentOTPException ex)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = string.Format(_localizer.GetString("err_user_otp_sent"), ex.RemainTime), Data = ex.RemainTime });
            }
            catch (MaximumOTPCountException)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("err_user_otp_sent_count") });
            }
        }

        /// <summary> For Change user password</summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     PUT/ChangePassword
        ///     {
        ///         "Username" : "raminazali9",
        ///         "Password":"12345678", => Password Mean new Password
        ///         "OTP":"30591" => OTP Sent You in Part ForgetPassword 
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Change Password</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 0 , Message = "Inavlid OTP or expired OTP" }
        ///     
        ///     { Code = 1 , Message = "No User Found" }
        ///     
        ///     { Code = 2 , Message = "An error occured in digi maliat" }
        /// </response>
        [HttpPut("ChangePassword")]
        public async Task<ActionResult> ChangePassword(UserForgetPassDTO userForgetPassDTO)
        {
            try
            {
                await _serviceWrapper.User.ChangePass(userForgetPassDTO.Username, userForgetPassDTO.Password, userForgetPassDTO.OTP);
                return Ok();
            }
            catch (InvalidOTPException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_user_otp") });
            }
            catch (UserNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("err_user_notFound") });
            }
            catch (DigiSignupException ex)
            {
                return BadRequest(new GenericMessage { Code = 2, Message = _localizer.GetString("err_user_password_update"), Data = new { ex.DigiMessage, ex.DigiCode } });
            }
        }

        /// <summary> For change user password from his/him profile, it means that user must logged in and token must be sent</summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     Description => Send Jwt-token In Request Header(Authrization) 
        ///     PUT/ProfileChangePassword
        ///     {
        ///         "OldPassword":"12345678", => Old Password
        ///         "NewPassword": "30591" => New Password 
        ///     }
        ///
        /// </remarks>
        /// <response code="200">profile Change Password</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 0 , Message="Username or Password Not Valid!" }
        /// </response>
        [Authorize]
        [HttpPut("ProfileChangePassword")]
        public async Task<ActionResult> ProfileChangePassword(ProfileChangePasswordDTO profileChangePasswordDTO)
        {
            try
            {
                await _serviceWrapper.User.ProfileChangePassword(User.GetUserId(), profileChangePasswordDTO.NewPassword, profileChangePasswordDTO.OldPassword);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_user_notFound") });
            }
        }

        /// <summary> Signup user and send OTP to user mobile </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///        "Username": "Ramin",
        ///        "Password": "123",
        ///        "Mobile" : "9655236532"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Signed Up</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 0, Message = "You have a digi maliat account." }
        ///     
        ///     { Code = 1, Message = "Your username is reserved in DG." } 
        ///     
        ///     { Code = 2, Message = "Your username is used by other." } 
        ///     
        ///     { Code = 3, Message = "An error occured in DG" } 
        /// </response>
        [HttpPost("Signup")]
        public async Task<ActionResult> Singup(UserSignupDTO userSignupDTO)
        {
            try
            {
                await _serviceWrapper.User.Signup(userSignupDTO.UserName, userSignupDTO.Password, userSignupDTO.Mobile);
                return Ok();
            }
            catch (UserHasDigiException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_user_signup_have_digi") });
            }
            catch (UsernameExistInDigiException)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("err_user_signup_username_reserved") });
            }
            catch (UsernameExistException)
            {
                return BadRequest(new GenericMessage { Code = 2, Message = _localizer.GetString("err_user_signup_username_used") });
            }
            catch (DigiSignupException ex)
            {
                return BadRequest(new GenericMessage { Code = 3, Message = _localizer.GetString("err_user_signup_digi"), Data = new { ex.DigiMessage, ex.DigiCode } });
            }
            catch (UserInActiveInDigiException)
            {
                return BadRequest(new GenericMessage { Code = 4, Message = _localizer.GetString("err_user_signup_inactive_digi") });
            }
        }

        /// <summary>For Change user mobile number from profile</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Description => Send Jwt-token In Request Header(Authrization) 
        ///     {
        ///      "Mobile": "9142652656"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Profile Change Mobile</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 0, Message = "User not found" }
        ///     
        ///     { Code = 1, Message = "An error occured in DG" }
        ///     
        ///     { Code = 2, Message = string.Format("You must wait for {0}", ex.RemainTime), Data = ex.RemainTime }
        /// </response>
        [Authorize]
        [HttpPut("ProfileChangeMobile")]
        public async Task<ActionResult> ProfileChangeModbile(ForgetPasswordMobileDTO forgetPasswordMobileDTO)
        {
            try
            {
                await _serviceWrapper.User.ChangeMobile(forgetPasswordMobileDTO.Mobile, User.GetUserId());
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_user_notFound") });
            }
            catch (UserUpdateMobileInDigiException)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("err_user_change_mobile") });
            }
            catch (UserSentOTPException ex)
            {
                return BadRequest(new GenericMessage { Code = 2, Message = string.Format(_localizer.GetString("err_user_otp_sent"), ex.RemainTime), Data = ex.RemainTime });
            }
        }

        /// <summary> After change mobile from profile, use this for verify mobile</summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Description => Send Jwt-token In Request Header(Authrization) 
        ///     {
        ///      "MobileOTP": "9142652656"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Verify Mobile Number Change</response>
        /// <response code="400">
        ///     Bad request for the following reasons :
        ///     
        ///     { Code = 0, Message = "User not found" }
        ///     
        ///     { Code = 1, Message = string.Format("You must wait for {0}", ex.RemainTime), Data = ex.RemainTime }
        /// </response>
        [Authorize]
        [HttpPost("VerifyChangeMobile")]
        public async Task<ActionResult> VerifyChangeMobile(ChangeMobileDTO changeMobileDTO)
        {
            try
            {
                await _serviceWrapper.User.VerifyChangeMobile(changeMobileDTO.MobileOTP, User.GetUserId());
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return BadRequest(new GenericMessage { Code = 0, Message = _localizer.GetString("err_user_notFound") });
            }
            catch (InvalidOTPException)
            {
                return BadRequest(new GenericMessage { Code = 1, Message = _localizer.GetString("err_user_otp") });
            }
        }
    }
}