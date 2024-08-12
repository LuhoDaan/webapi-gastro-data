FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80



# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Copy the csproj file and restore any dependencies
COPY ./GastroApi/*.csproj ./
RUN dotnet restore 
COPY . .
WORKDIR /src/GastroApi
RUN dotnet build GastroApi.csproj -c Release -o /app
# Use the official .NET runtime image to run the application
FROM build AS publish
RUN dotnet publish GastroApi.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Expose the API port
EXPOSE 80
# Set the entry point for the container
ENTRYPOINT ["dotnet", "GastroApi.dll"]
