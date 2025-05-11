# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Install Node.js, Allure CLI, and Java
RUN apt-get update && apt-get install -y \
    nodejs \
    npm \
    python3 \
    openjdk-17-jre-headless && \
    npm install -g allure-commandline && \
    apt-get clean

# Set JAVA_HOME environment variable
ENV JAVA_HOME=/usr/lib/jvm/java-17-openjdk-amd64
ENV PATH=$JAVA_HOME/bin:$PATH

RUN echo "Building the application11111111111111111........"
# Debug: Print JAVA_HOME to verify
RUN echo "JAVA_HOME is set to: $JAVA_HOME" && ls -la $JAVA_HOME
RUN echo "Building the application2222222222........"


# Copy the project files
COPY BookerApiTest/*.csproj ./BookerApiTest/
COPY BookerApiTest/ ./BookerApiTest/
WORKDIR /app/BookerApiTest
RUN ls -la
RUN pwd

RUN dotnet restore


# Copy the rest of the application files
#RUN dotnet build --configuration Release --output /app/build

# RUN dotnet test