rootName := "Things"
projectFolders = $(shell find -maxdepth 0 -prune -print | grep -o 'Things..[[:alnum:]]*')
publishTargets = $(addprefix 'publish_', ${projectFolders})
copyTargets = $(addprefix 'copy_', ${projectFolders})

.PHONY: list
list:
	@grep '^[^#[:space:]].*:' Makefile

clean:
	find . -type d -name "bin" -print0 | xargs -0 rm -rf
	find . -type d -name "obj" -print0 | xargs -0 rm -rf
	@echo cleaned.
	@echo $(branch)

# Command for building.
build: clean restore
	find . -name "*.csproj" -print | xargs -n1 dotnet build -nologo /clp:NoSummary --no-restore /property:GenerateFullPaths=true

test: build
	find . -name "*.csproj" -print | grep -i test | xargs -n1 dotnet test

# Commands for publishing.
cleanPkg:
	rm -rf pkg
	mkdir -p ./pkg

# Commands for restore.
restore:
	find . -name "*.csproj" -print | xargs -n1 dotnet restore -s https://pkgs.dev.azure.com/claros-devops/claros-nuget/_packaging/claros-nuget/nuget/v3/index.json -nologo /clp:NoSummary /property:GenerateFullPaths=true
	@echo restored.
	ifneq (,$(findstring sprint/Release,$(branch)))
		@echo found.
	else
		@echo not found.
	endif

${publishTargets}:
	@echo publish - $@
	dotnet restore --ignore-failed-sources -s https://pkgs.dev.azure.com/claros-devops/claros-nuget/_packaging/claros-nuget/nuget/v3/index.json ./src/$(subst 'publish_',,$@)
	dotnet publish -r win-x64 -c Release /clp:NoSummary ./src/$(subst 'publish_',,$@)


${copyTargets}:
	@echo copy - $(subst 'copy_',,$@)
	@mkdir -p ./pkg/$(subst 'copy_',,$@)Pkg
	@cp -rv ./src/$(subst 'copy_',,$@)/PackageRoot/* ./pkg/$(subst 'copy_',,$@)Pkg/
	@mv ./src/$(subst 'copy_',,$@)/bin/x64/Release/netcoreapp3.1/win-x64/publish ./pkg/$(subst 'copy_',,$@)Pkg/Code


copy: cleanPkg ${copyTargets}

publish: build ${publishTargets}
	@echo Done publishing.
