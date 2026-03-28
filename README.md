# Simple Gym Dashboard

This repository contains a .NET project to track the number of free spots available in gyms.

Right now, I implemented the client + tracking only for gym apps using the "Wellness in Cloud" system.

*Disclaimer: the client library to wellness in cloud API is an unofficial, independent project used for educational purposes, and not affiliated with the developers of Wellness in Cloud; use at your own risk*


## Local Development

### Environment Setup
The application needs a PostgreSQL database to work.

There is a `docker-compose.yml` file that can be used for local development.

### Application Setup
To setup the application, create the encryption key and IV for column encryption.
The Key must be 16,24 or 32 characters long, while the IV must be 16 bytes.
Then, write the base64-encoded values in your [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-10.0&tabs=windows#enable-secret-storage).

E.g.
```
"Encryption": {
    "Key": "your-key", 
    "InitializationVector": "your-iv"
},
```
