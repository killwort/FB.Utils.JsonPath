if exist .lock goto end
echo > .lock
for %%D in ("Language\*.atg") DO Language\Coco.exe %%D -frames Language -namespace FB.Utils.JsonPath.Language
del /q .lock
:end