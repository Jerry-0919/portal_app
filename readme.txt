DEVELOPMENT
cd diga.web
npm run serve
dotnet watch run

PUBLICATION
cd diga.web
npm run build
rm -r dist/*
dotnet publish -o dist
rsync -av dist/* root@51.254.35.145:/var/www/new.diga.pt
password
ssh root@51.254.35.145
password
sudo systemctl restart kestrel-helloapp.service
sudo chmod -R 777 /var/www/new.diga.pt/wwwroot