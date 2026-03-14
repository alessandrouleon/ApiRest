## CRIAR O BANCO

#### docker compose up -d

## Cria migration

### dotnet ef migrations add CreateUser --project src/Infrastructure --startup-project src/Api

## Rodar atualizar tabela no banco

### dotnet ef database update --project src/Infrastructure --startup-project src/API

## Iniciar projeto

### dotnet run --project src/API

## Iniciar projete, abre dirento o swagger

### dotnet watch --project src/Api
