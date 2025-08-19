# ---------- مرحله Build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 
COPY . .

# به جای Myrestfulapi.csproj نام واقعی پروژهٔ 
RUN dotnet restore "./MyRestfulApi/.csproj"
RUN dotnet publish "./MyRestfulApi.csproj" -c Release -o /app/publish

# ---------- مرحله Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .


ENTRYPOINT ["dotnet", "Myrestfulapi.dll"]
