
## Run:
> dotnet run --project src/backend/AS-2025.csproj

## Test:
> hurl --test src/test/login.hurl

## Infra (postgres, pgadmin, minio)
> .env.example -> .env
> docker-compose up -d

## Scalar OpenAPI
> https://localhost:58708/scalar

## Docker build:
> docker build -t as2025 .
> docker run -p 8080:8080 -p 8081:8081 as2025

## Down

> docker compose down -v

## Wipe everything

docker system prune -a