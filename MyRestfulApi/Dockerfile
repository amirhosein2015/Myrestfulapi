# ---------- مرحله Build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# کل سورس را کپی کن
COPY . .

# به جای Myrestfulapi.csproj نام واقعی پروژهٔ خودت را بگذار
RUN dotnet restore "./Myrestfulapi.csproj"
RUN dotnet publish "./Myrestfulapi.csproj" -c Release -o /app/publish

# ---------- مرحله Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# به جای Myrestfulapi.dll نام واقعی DLL پروژهٔ خودت را بگذار
ENTRYPOINT ["dotnet", "Myrestfulapi.dll"]
