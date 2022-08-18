MAKEFLAGS += --silent

verify:
	./scripts/verify.sh

example-01:
	dotnet run --project src/01-no-encryption/NoEncryption

example-02:
	dotnet run --project src/02-basic-encryption/BasicEncryption

example-03:
	docker-compose -f src/03-shared-encryption/docker-compose.yml --project-directory src/03-shared-encryption down -v
	docker-compose -f src/03-shared-encryption/docker-compose.yml --project-directory src/03-shared-encryption up -d --build

example-04:
	docker-compose -f src/04-key-rotation/docker-compose.yml --project-directory 04-key-rotation down -v
	docker-compose -f src/04-key-rotation/docker-compose.yml --project-directory 04-key-rotation up -d --build

example-05:
	docker-compose -f src/05-openkms-encryption/docker-compose.yml --project-directory src/05-openkms-encryption down -v
	docker-compose -f src/05-openkms-encryption/docker-compose.yml --project-directory src/05-openkms-encryption up -d --build
