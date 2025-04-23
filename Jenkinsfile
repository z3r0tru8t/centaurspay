pipeline {
    agent any

    stages {
        stage('Build') {
            steps {
                echo '🔧 Building the project...'
                sh 'dotnet build'
            }
        }

        stage('Test') {
            steps {
                echo '✅ Running tests...'
                sh 'dotnet test --no-build'
            }
        }

        stage('Publish') {
            steps {
                echo '📦 Publishing artifact...'
                sh 'dotnet publish -c Release -o ./out'
            }
        }

        stage('Docker Build & Run') {
            steps {
                echo '🐳 Docker image oluşturuluyor...'
                sh '''
                    docker build -t centaurspay-api .
                    docker stop centaurspay-api || true
                    docker rm centaurspay-api || true
                    docker run -d -p 8082:8080 --name centaurspay-api centaurspay-api
                '''
            }
        }

       stage('SonarQube Analysis') {
    environment {
        SONAR_TOKEN = credentials('sonarqube-token') // bu doğru
    }
    steps {
        withSonarQubeEnv('SonarQube') {  // tam olarak böyle yazılmalı
            sh 'dotnet sonarscanner begin /k:"centaurspay-api" /d:sonar.login=$SONAR_TOKEN'
            sh 'dotnet build'
            sh 'dotnet sonarscanner end /d:sonar.login=$SONAR_TOKEN'
        }
    }
}
    }
}
