# Number to Words Converter

A simple web application that converts numeric values into their corresponding word representation. The application supports both positive and negative numbers, as well as decimal values up to two decimal places. 

To use the app, visit the following link in your browser:
[**Live Demo**](https://techtest-2vrm.onrender.com/)

⚠️ *Please allow up to 50 seconds for the server instance to spin up from cold start if needed.*

## Build Solution Locally

### Clone and Run the Repository
```sh
git clone https://github.com/ejbolts/TechTest
cd TechTest
dotnet run
```
The application should be hosted at `http://localhost:5154` 

### Run Using Docker
```sh
docker build -t techtest-image .
docker run -d --rm -p 8080:8080 --name techtest-container techtest-image
```
The application should be hosted at `http://localhost:8080` 

