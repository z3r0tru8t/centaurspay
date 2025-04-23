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
    }
}
