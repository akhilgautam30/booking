# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files
COPY BookerApiTest/*.csproj ./BookerApiTest/
COPY BookerApiTest/ ./BookerApiTest/
WORKDIR /app/BookerApiTest
RUN echo "Building the application2222222222........"
RUN ls -la
RUN pwd

RUN dotnet restore


# Copy the rest of the application files
RUN dotnet build --configuration Release --output /app/build

RUN dotnet test

# # Publish the application
# RUN dotnet publish --configuration Release --output /app/publish

# # Use the runtime image for running the application
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
# WORKDIR /app
# COPY --from=build /app/publish .

# # Install Node.js and Allure CLI
# RUN apt-get update && apt-get install -y \
#     nodejs \
#     npm \
#     python3 \
#     && npm install -g allure-commandline \
#     && apt-get clean

# # Copy the test results directory
# COPY --from=build /app/build/bin/Debug/net8.0 /app/test-results

# # Run tests, generate Allure report, and serve it
# CMD dotnet test --logger "trx;LogFileName=test-results.trx" && \
#     allure generate /app/test-results/allure-results -o /app/allure-report --clean && \
#     npx http-server /app/allure-report -p 8000