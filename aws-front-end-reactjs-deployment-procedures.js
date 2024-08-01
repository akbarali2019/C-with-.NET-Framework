// AWS Lighsail Instance React.js Deployment
//
//1. Login to AWS;
//2. Go to Lightsail;
//3. Create Instance:
//	-Select instance Location; 
//	-OS only;
//	-Ubuntu 20.04 LTS;
//	-Instance Plan;
//	-Identify the instance -> for ex. kefa-erp-lab
//4. Set the Networking by adding HTTPS TCP 443;
//5. If the domain is registered already set DNS records by rooting IP address of this instnace:
//	Record type -> A record
//	Record name -> @
//	Resolves to -> instance IP address
//6. Install FileZilla into local PC and connect to the instance using SSH Key;
//7. After connection upload built react.js dist folder into /home/ubuntu;
//8. Connect to the instance terminal from AWS;
//9. Follow the steps by the following commands in the terminal:
//	1. Insall NGINX
//		- sudo apt-get update
//		- sudo apt-get install nginx
//		- sudo ufw allow 'Nging Full'
//		
//	2. Install Node.js and NPM
//		- sudo apt-get install nodejs
//		- sudo apt-get install build-essential
//		
//	3. Link the Project's Build Folder in the Server
//		- cd /var/www/
//		- sudo mkdir kefalab.com
//			-> make a symbolic link to your project's build folder with the ln -s command:
//		- sudo ln -s /home/ubuntu/dist/* /var/www/kefalab.com/
//		
//			OR    -> in case any error, just move or copy dist files into /var/www/kefalab.com/
//		- sudo rm -rf /var/www/kefalab.com/*
//		- sudo mv /home/ubuntu/dist/* /var/www/kefalab.com/ OR sudo cp -R /home/ubuntu/dist/* /var/www/kefalab.com/
//
//		- sudo chown -R www-data:www-data /var/www/kefalab.com  (ensures proper permissions for the web server)
//		- sudo chmod -R 755 /var/www/kefalab.com
//		
//	4. Modify the NGINX Configuration Files
//		1. Check sites-enabled and sites-available folders to reomve default configuration
//		- sudo ls /etc/nginx/sites-available/
//		-> if there is a default file, remove it
//		- sudo rm /etc/nginx/sites-available/default
//		
//		- sudo ls /etc/nginx/sites-enabled/
//		-> if there is a default file, remove it
//		- sudo rm /etc/nginx/sites-enabled/default
//		
//		2. Create custom nginx proxy configurations
//		- sudo nano /etc/nginx/sites-available/kefalab.com
//		
//			-> write the rules like:
//			
//			server {
//
//				listen 80;
//
//				listen [::]:80;
//
//				server_name kefalab.com www.kefalab.com;
//
//				location / {
//
//					return 301 https://$host$request_uri;
//
//				}
//
//			}
//
//
//			server {
//
//				listen 443 ssl;
//
//				listen [::]:443 ssl;
//
//				server_name kefalab.com www.kefalab.com;
//
//				root /var/www/kefalab.com;
//
//				index index.html;
//
//				ssl_certificate /etc/letsencrypt/live/kefalab.com/fullchain.pem;
//
//				ssl_certificate_key /etc/letsencrypt/live/kefalab.com/privkey.pem;
//
//				include /etc/letsencrypt/options-ssl-nginx.conf;
//
//					ssl_dhparam /etc/letsencrypt/ssl-dhparams.pem;
//
//
//				location / {
//
//					try_files $uri $uri/ /index.html;
//				}
//
//
//				location /api/v1/ {
//
//					proxy_pass http://13.124.1.149:8080/api/v1/;
//
//					proxy_http_version 1.1;
//
//					proxy_set_header Upgrade $http_upgrade;
//
//					proxy_set_header Connection 'upgrade';
//
//					proxy_set_header Host $host;
//
//					proxy_cache_bypass $http_upgarde
//
//					proxy_set_header X-Real-IP $remote_addr;
//
//					proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
//
//					proxy_set_header X-Forwarded-Proto $scheme;
//
//					proxy_set_header Authorization $http_authorization;
//
//				}
//
//			}
//			
//			-> save and exit (Ctrl+s and Ctrl+x)
//			
//		3. Link Configuration File:
//		
//		- sudo ln -s /etc/nginx/sites-available/kefalab.com /etc/nginx/sites-enabled/kefalab.com
//		-> check directed location to confirm
//		
//		4. Test Nginx Configuration
//		
//		- sudo nginx -t
//		-> If the configuration test is successful, you should see output similar to:
//		--> nginx: the configuration file /etc/nginx/nginx.conf syntax is ok
//        --> nginx: configuration file /etc/nginx/nginx.conf test is successful
//		
//		- sudo systemctl restart nginx (If the configuration test is successful, you can restart Nginx to apply the changes)
//		- sudo systemctl status nginx
//
//10.Obtain SSL Certificate with Let's Encrypt
// 	
//	1. Add the CertBot repository for the latest version:
//		- sudo add-apt-repository ppa:certbot/certbot
//
//	2. Update the package list afterwards to pick up the new information:
//		- sudo apt-get update
//		
//	3. Lastly, install CertBot's python/NGINX package:
//		- sudo apt-get install python3-certbot-nginx	
//
//	4. This plugin will do most of the work needed to get and maintain a security certificate, including modifying the NGINX configuration files and reloading when the certificate eventually expires. To run CertBot, typ**:
//        - sudo certbot --nginx -d kefalab.com -d www.kefalab.com
//
//	5. Enter your email address and agree to the terms of service if required. CertBot will then present you with some options
//
//	6. Redo Section 9.4 procedures in the above to test and restart NGINX
//
//	7. Access the Website
//		-> Open your web browser and navigate to your website to ensure it is loading correctly (https://www.kefalab.com).

// Helpful links
// https://github.com/Olafaloofian/React-Frontend-Lightsail-Deployment
// https://chatgpt.com/c/32332b72-2dd9-43ac-b032-1d7740f6b3db



//////// ****** UPDATE PROCEDURE ******

//***********************************************************************backend-->

//Reboot the backend instance and open the instance console window
// clear
// ls

//delete running old files
// sudo rm -rf /home/ubuntu/logs
// sudo rm -rf /home/ubuntu/nohup.out
// sudo rm -rf /home/ubuntu/erpV8.jar

//check the deletion
// ls

//Connect to FileZilla  to upload a new .jar file

//Check the server to ensure the new .jar file has been uploaded
// ls

//Run the server Permanantly
// nohup java -jar erpV9.jar &

//Exit the console window


//***********************************************************************frontend-->

//open the instance console window

// clear
// ls /home/ubuntu/dist/                 (it should be empty)
// ls /var/www/kefalab.com/              (it should contain assets, index.html file)
// sudo rm -rf /var/www/kefalab.com/*    (we delete these files to change with new ones)
// ls /var/www/kefalab.com/              (now it should be empty)

//Connect to FileZilla  to upload a new dist files -> assests folder, and index.html

// ls /home/ubuntu/dist/                 (it should contain new uploaded assets folder and index.html file)

//Move new files into /var/www/kefalab.com/
// sudo mv /home/ubuntu/dist/* /var/www/kefalab.com/

//Check new files
// ls /var/www/kefalab.com/

//Check NGINX
// sudo nginx -t
// sudo systemctl restart nginx 
// sudo systemctl status nginx