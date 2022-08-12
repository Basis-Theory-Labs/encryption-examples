MAKEFLAGS += --silent

example-01:
	dotnet run --project 01-no-encryption/NoEncryptio

example-02:
	dotnet run --project 02-basic-encryption/BasicEncryption

example-03:
	docker-compose -f 03-shared-encryption/docker-compose.yml --project-directory 03-shared-encryption down -v
	docker-compose -f 03-shared-encryption/docker-compose.yml --project-directory 03-shared-encryption up -d --build

example-05:
	docker-compose -f 05-openkms-encryption/docker-compose.yml --project-directory 05-openkms-encryption down -v
	docker-compose -f 05-openkms-encryption/docker-compose.yml --project-directory 05-openkms-encryption up -d --build
