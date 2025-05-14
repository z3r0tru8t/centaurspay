# centaurspay-api
🔧 How to Set Up Environment Variables
To run this project successfully, you need to set up a .env file with your API keys and secrets.

📍 Where to create the .env file?
Create the .env file in the root folder of the project — the same folder where the .sln or .csproj file is located.

📝 What to put inside the .env file?
Add the following lines to your .env file and replace the values with your own credentials:


JWT_KEY= YOUR KEY
GOOGLE_CLIENT_ID=YOUR KEY
GOOGLE_CLIENT_SECRET=YOUR KEY

🔑 How to Get Your API Keys
To use this project, you need to provide some environment variables. Here's how you can get them:

1. JWT_KEY
This is a secret key used to sign your JWT tokens.

You can generate a strong key manually using an online password generator (e.g., LastPass Password Generator)

🔐 Make sure this key is long and complex enough to ensure security.

2. GOOGLE_CLIENT_ID and GOOGLE_CLIENT_SECRET
These are required for Google login integration.

Steps to get them:
Go to the Google Cloud Console

Create a new project (or select an existing one)

Go to APIs & Services > OAuth consent screen and configure the app info

Then go to Credentials and click Create Credentials > OAuth Client ID

Choose Web application and set your authorized redirect URIs (e.g., http://localhost:5000/signin-google)

Once created, you'll get the Client ID and Client Secret

Add them to your .env file like this:

GOOGLE_CLIENT_ID=YOUR KEY
GOOGLE_CLIENT_SECRET=YOUR KEY

✅ With these values set in your .env file, you're ready to run the project!