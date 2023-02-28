## Demo
http://luizalabs.s3-website-us-east-1.amazonaws.com/


## Rodar local
### BackEnd
```sh
cd luizalabs-api/luizalabs-api-sln/luizalabs-api
docker-compose up -d
dotnet restore
dotnet ef database update
dotnet run
```
### FrontEnd
```sh
cd luizalabs-app && yarn && yarn serve
```

## Tecnologias utilizadas
- .Net Core 7
- MySQL
- VUE 3
- AWS S3
- AWS EC2

## Design pattern
- Code First
- MVC com repositório
- Teste de integração

## Modelo do banco de dados
| User |
|---|
|Name|
|Email|
|Password|
|IsVerified|
|Status|
|CreatedAt|
|UpdatedAt|
----
| UserVerification |
|---|
|Token|
|TwoFactorToken|
|UserId|
|Used|
|CreatedAt|
|UpdatedAt|