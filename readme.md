
### Start
> docker compose up -d --build

#### Links
| | | | |
|-|:-:|:-:|:------: |
PGAdmin | http://localhost:5050 |admin@pg.com | admin
MINIO | http://localhost:9001 | minioadmin | minioadmin
Scalar | http://localhost:5002/scalar

### Down
> docker compose down -v

### Unit test:
> dotnet test

### Integration test:
> hurl --insecure --test **/*.hurl --variables-file ./src/test/.env.local --variable hostname=localhost

### Wipe everything
> docker system prune -a