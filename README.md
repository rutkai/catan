Settlers of Catan
=================

Fejlesztés
----------

1. Buildeld a projektet (ha nem létezne, akkor jöjjön létre a bin/debug könyvtár)
2. Másold a bin/debug könyvtárba az installer könyvtárban lévő dll-eket és az Image könyvtárat

Telepítő fordítása
------------------

Előkészületek: Töltsd le és telepítsd az NSIS telepítőt
1. Frissítsd a dll fájlokat és Image könyvtárat
2. Ha szükséges, másolj az Installer könyvtárba további fájlokat (ezeket az installer.nsi fájlban be kell venni a telepítőbe!)
3. Másold a bin/release (!!!) könyvtárból az Installer könyvtárba a Catan.exe állományt
4. Fordítsd le az installert készítő installer.nsi fájlt (jobb klikk, Compile)