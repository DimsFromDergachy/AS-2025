### Start
> docker compose up -d --build

#### Links
| Service |             Url              |    Login     |  Password  |
| ------- | :--------------------------: | :----------: | :--------: |
| PGAdmin |    http://localhost:5050     | admin@pg.com |   admin    |
| MINIO   |    http://localhost:9001     |  minioadmin  | minioadmin |
| Scalar  | http://localhost:5002/scalar |              |            |
### Down
> docker compose down -v

### Unit test:
> dotnet test

### Integration test:
> hurl --insecure --test **/*.hurl --variables-file ./src/test/.env.local --variable hostname=localhost

### Wipe everything
> docker system prune -a

### Users
| Email             | Username | Password    | Roles         |
| ----------------- | -------- | ----------- | ------------- |
| admin@test.com    | admin    | Password123 | Administrator |
| manager1@test.com | manager1 | Password123 | Manager       |
| user1@test.com    | user1    | Password123 | User          |
| user2@test.com    | user2    | Password123 | User          |
