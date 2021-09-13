    pipeline {
        agent any

        stages {
            stage('build') {
                steps {
                    echo "Current workspace is ${env.WORKSPACE}"
                    sh 'dotnet restore HasebCoreApi//HasebCoreApi.csproj && dotnet clean HasebCoreApi//HasebCoreApi.csproj  && dotnet build HasebCoreApi//HasebCoreApi.csproj --configuration Release && dotnet test HasebCoreApi//HasebCoreApi.csproj && dotnet publish  HasebCoreApi//HasebCoreApi.csproj  && rm -rf /var/jenkins_home/jobs/haseb/builds &&  cp -r HasebCoreApi/bin/Release/netcoreapp3.1/publish/ /var/jenkins_home/jobs/haseb/builds/'

                }
            }
        }
    }