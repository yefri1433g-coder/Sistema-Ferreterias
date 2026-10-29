# Comandos rápidos

## Compilar y revisar el Panel — Documento 07

Repositorio actual verificado: `C:\2026_ProgramacionII`.
Ejecutar en PowerShell:

```powershell
Set-Location C:\2026_ProgramacionII
& 'C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe' `
  '.\src\SistemaFerreterias\SistemaFerreterias.slnx' /t:Build /p:Configuration=Debug `
  /p:OutDir=C:\2026_ProgramacionII\src\SistemaFerreterias\Presentacion\bin\CodexPanelRevision\ /v:minimal /nologo
& '.\src\SistemaFerreterias\Presentacion\bin\CodexPanelRevision\Presentacion.exe'
```

La aplicación mantiene su inicio por Login. Tras entrar, probar los enlaces del
Panel, repetir una opción, regresar a Dashboard y maximizar/restaurar. En ventanas
pequeñas, usar las barras de desplazamiento del menú y del contenedor.
`Salir` termina la aplicación. El mapa está en `skills/08_Panel_Navegacion.md`.

Para revisar únicamente los cambios del Panel:

```powershell
git diff -- src/SistemaFerreterias/Presentacion/MenuPrincipalForms.cs src/SistemaFerreterias/Presentacion/MenuPrincipalForms.Designer.cs
git diff --check
```

## Estado del repositorio compartido

Verificado el 23 de octubre de 2026:

- Clon: `C:\2026_Progra2_Integracion`.
- Remoto configurado: `https://github.com/yefri1433g-coder/Sistema-Ferreterias.git`.
- `main` estaba sincronizada con `origin/main`.
- Ramas remotas observadas: `integrante1`, `integrante2` e `Integrante-3`.
- El historial muestra integrados los Pull Requests #1, #2 y #4.

La URL y el historial no permiten verificar desde el clon la visibilidad privada actual del repositorio.

## Actualizar `main`

Antes de cambiar de rama, revisa si tienes cambios locales. No los descartes para actualizar.

```cmd
cd /d C:\2026_Progra2_Integracion
git status --short --branch
git switch main
git pull origin main
```

## Crear una rama para el siguiente trabajo

Usa un nombre propio y descriptivo; crea la rama desde `main` actualizado.

```cmd
git switch -c integrante2-descripcion-tarea
git branch --show-current
```

## Revisar y publicar cambios

Agrega rutas revisadas explícitamente. No uses `git add .` automáticamente.

```cmd
git status --short
git diff --stat
git diff
git add ruta\del\archivo1 ruta\del\archivo2
git diff --cached --stat
git diff --cached
git status
git commit -m "Descripción breve del cambio"
git push -u origin nombre-de-la-rama
```

Después, crea un Pull Request en GitHub con `main` como base y tu rama como compare. Revisa los archivos modificados con el grupo antes de integrar.

## Confirmar rama y remoto

```cmd
git branch --show-current
git branch -vv
git remote -v
git status --short --branch
```
