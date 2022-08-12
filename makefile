MAKEFLAGS += --silent

example-01:
	dotnet run --project 01-no-encryption/NoEncryptio

example-02:
	dotnet run --project 02-basic-encryption/BasicEncryption

example-03:
	docker-compose -f 03-shared-encryption/docker-compose.yml --project-directory 03-shared-encryption down -v
	docker-compose -f 03-shared-encryption/docker-compose.yml --project-directory 03-shared-encryption up -d --build

example-04:
	docker-compose -f 04-shared-encryption/docker-compose.yml --project-directory 04-shared-encryption down -v
	docker-compose -f 04-shared-encryption/docker-compose.yml --project-directory 04-shared-encryption up -d --build