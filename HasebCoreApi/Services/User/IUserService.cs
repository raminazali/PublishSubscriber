using System;
using System.Threading.Tasks;
using HasebCoreApi.Models;

namespace HasebCoreApi
{
    public interface IUserService
    {
        /// <summary>
        /// Authenticate user by username and password
        /// Send OTP to user mobile if authenticate is success
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="mobile"></param>
        /// <exception cref="UserInActiveException">User inactive in haseb</exception>
        /// <exception cref="UserNotFoundException">Throw when user not found in digi and haseb</exception>
        /// <exception cref="UserHasNoMobileInDigiException">Throw when user's mobile not validate in digi</exception>
        /// <exception cref="UserUpdateMobileInDigiException">Throw when an error occured in digi mobile update</exception>
        /// <exception cref="UserSentOTPException">Throw when OTP has been sent and user must wait for (n) min</exception>
        /// <returns></returns>
        Task<(UserInfo, bool)> Authenticate(string username, string password);
        //string mobile
        /// <summary>
        /// SignUp With Mobile,Username and password and Check digimaliat and when he/she doesnt signuped in digimaliat 
        /// its will be signup in digimaliat and digihaseb
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="mobile"></param>
        /// <exception cref="UsernameExistException">throw when Username Exist, Username must be Unique </exception>
        /// <exception cref="UsernameExistInDigiException">Throw when username has used before in digi</exception>
        /// <exception cref="DigiSignupException">Throw Digi SignUp went wrong</exception>
        /// <exception cref="UserHasDigiException">if user has digi, should be login with digi credentials</exception>
        /// <returns></returns>
        Task Signup(string username, string password, string mobile);

        /// <summary>
        /// Verify an user with otp and validate him/her mobile number if signup is true the user is registered and should update validation in haseb and digi. 
        /// </summary>
        /// <exception cref="UserNotFoundException">Throw when username not found in db.</exception>
        /// <exception cref="UserInActiveException">Throw when user is not avtive.</exception>
        /// <exception cref="InvalidOTPException">Throw when otp is not valid.</exception>
        /// <exception cref="UserValidateMobileInDigiException">Throw when an errro occured in digimaliat in validate mobile.</exception>
        /// <param name="username"></param>
        /// <param name="otp"></param>
        /// <param name="signup"></param>
        /// <returns></returns>
        Task<(string token, UserInfo user)> Verify(string username, string otp, bool signup, bool ForgetPassword, bool active);

        /// <summary>
        /// Change Password from profile (change will be applied in Digimaliat too)
        /// </summary>
        /// <param name="username"></param>
        /// <param name="newpassword"></param>
        /// <param name="oldpassword"></param>
        /// <param name="userid"></param>
        /// <exception cref="UserNotFoundException">throw when user equal null or old password is Wrong </exception>
        /// <returns></returns>
        Task ProfileChangePassword(string userid, string newpassword, string oldpassword);

        /// <summary>
        /// Ckecking the Entered Username Or Email Or Mobile Number, And If Its Exists.
        /// </summary>
        /// <exception cref="UserNotFoundException">Throw When Username Is Not Valid(Username Not Equals With username in database). </exception>
        /// <exception cref="UserSentOTPException">Throw when OTP has been sent and user must wait for (n) min</exception>
        /// <exception cref="MaximumOTPCountException">Throw when OTP has been sent more than specified</exception>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<UserInfo> ForgetPassword(string username);

        /// <summary>
        ///  Give the new Password For Update old Password With New Password.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="otp"></param>
        /// <exception cref="UserNotFoundException">Throw When Username Is Not Valid(Username Not Equals With username in database). </exception>
        /// <exception cref="InvalidOTPException">Throw when OTP Time Finished Or OTP Not True. </exception>
        /// <exception cref="DigiSignupException">Throw When User Recives Code 0 From DigiMaliat Server . </exception>
        /// <returns></returns>
        Task ChangePass(string username, string password, string otp);

        /// <summary>
        ///  Change Or Add Email To UserInfo .
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userid"></param>
        /// <exception cref="UserNotFoundException">Throw when User Not Exist With Specific UserId. </exception>
        /// <returns></returns>
        Task ChangeEmail(string email, string userid);

        /// <summary>
        ///  Verify EmailOtp and If Its True Change EmailValidate False To True.
        /// </summary>
        /// <param name="emailOTP"></param>
        /// <param name="userid"></param>
        /// <exception cref="InvalidOTPException">Throw when Email OTP or Time Is Invalid. </exception>
        /// <exception cref="UserNotFoundException">Throw When User Not Exist With Specific UserId. </exception>
        /// <returns></returns>
        Task VerifyEmail(string emailOTP, string userid);

        /// <summary>
        ///  Verify EmailOtp and If Its True Change EmailValidate False To True.
        /// </summary>
        /// <param name="mobile"></param> 
        /// <param name="userid"></param>
        /// <exception cref="UserUpdateMobileInDigiException">Throw when an error occured in digi mobile update</exception>
        /// <exception cref="UserNotFoundException">Throw When User Not Exist With Specific UserId. </exception>
        /// <exception cref="UserSentOTPException">Throw when OTP has been sent and user must wait for (n) min</exception>
        /// <returns></returns>
        Task ChangeMobile(string mobile, string userid);

        /// <summary>
        ///  Verify Mobile OTP 
        /// </summary>
        /// <param name="mobileOTP"></param>
        /// <param name="userid"></param>
        /// <exception cref="InvalidOTPException">Throw when Email OTP or Time Is Invalid. </exception>
        /// <exception cref="UserNotFoundException">Throw When User Not Exist With Specific UserId. </exception>
        /// <returns></returns>
        Task VerifyChangeMobile(string mobileOTP, string userid);
    }

}