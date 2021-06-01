#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
#WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN curl -sL https://deb.nodesource.com/setup_14.x |  bash -
RUN apt-get update
RUN apt-get install -y nodejs

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
RUN curl -sL https://deb.nodesource.com/setup_14.x |  bash -
RUN apt-get update
RUN apt-get install -y nodejs 
RUN npm install -g npm
#WORKDIR ./../../frontend
#RUN npm run build
#COPY 
WORKDIR /src
#RUN bash -c "echo -e test"
#COPY ./backend .
COPY ["./backend/Expenses.Api/", "Expenses.Api/"]
COPY ["./backend/Expenses.Application/", "Expenses.Application/"]
COPY ["./backend/Expenses.Domain/", "Expenses.Domain/"]
COPY ["./backend/Expenses.Infrastructure/", "Expenses.Infrastructure/"]
COPY ["./frontend/", "frontend/"]
RUN cd frontend && npm install
RUN cd frontend && npm run build
RUN dotnet restore "Expenses.Api/Expenses.Api.csproj"
#COPY . .
#WORKDIR "/src"
RUN dotnet build "Expenses.Api/Expenses.Api.csproj" -c Release -o /app/build
#
FROM build AS publish
RUN dotnet publish "Expenses.Api/Expenses.Api.csproj" -c Release -o /app/publish
RUN mkdir ../app/publish/ClientApp
RUN mv frontend/build ../app/publish/ClientApp

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Expenses.Api.dll"]