
### Start
> docker compose up -d

#### Links
| | | | |
|-|:-:|:-:|:------: |
PGAdmin | http://localhost:5050 |admin@pg.com | admin
MINIO | http://localhost:9001 | minioadmin | minioadmin
Scalar | https://localhost:58708/scalar

### Down
> docker compose down -v

### Test:
> hurl --test **/*.hurl

### Docker build:
> docker build -t as2025 .
> docker run -p 8080:8080 -p 8081:8081 as2025

### Wipe everything
> docker system prune -a