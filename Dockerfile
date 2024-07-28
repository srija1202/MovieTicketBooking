# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["MovieTicketBooking/MovieTicketBooking.csproj", "MovieTicketBooking/"]
RUN dotnet restore "MovieTicketBooking/MovieTicketBooking.csproj"

# Copy everything else and build
COPY MovieTicketBooking/. MovieTicketBooking/
WORKDIR "/src/MovieTicketBooking"
RUN dotnet build "MovieTicketBooking.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "MovieTicketBooking.csproj" -c Release -o /app/publish

# Build the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MovieTicketBooking.dll"]
