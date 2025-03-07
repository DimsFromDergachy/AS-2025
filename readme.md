
### Start
> docker compose up -d

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
> hurl --test **/*.hurl

### Wipe everything
> docker system prune -a