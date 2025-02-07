# Use the official .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0.405 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the .csproj file and restore the dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application files
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Use the official .NET runtime image for the final stage (smaller image)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the built app from the build stage
COPY --from=build /app/out ./

# Expose the port that your app will listen on
EXPOSE 80

# Set the entry point to run your application
ENTRYPOINT ["dotnet", "backend.dll"]
