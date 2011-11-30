#!/bin/bash

#This script grabs the authorization token from linkedin
LYNX_COOKIES="/home/brza/.lynx_cookies"
WGET_COOKIES="cookies_wget.txt"
URL="www.linkedin.com"
JSESSION_TEXT="jsession.txt"
LEO_AUTH_TOKEN_TEXT="leo_auth_token.txt"
LYNX_CMD_SCRIPT="/home/brza/bin/get_linkedin_credentials/linkedin_script.txt"
LINKEDIN_MINER_DIR="$HOME/bin/LinkedInMiner"

#Remove any existing session cookies
touch $LYNX_COOKIES
rm $LYNX_COOKIES

#Use wget to retrive the jsession id for the login
wget --keep-session-cookies --save-cookies $WGET_COOKIES $URL

#Extract the jsession id into a file
cat $WGET_COOKIES | grep JSESSIONID | cut -f7 > $JSESSION_TEXT

echo `pwd`

#Use lynx to get the leo_auth_token
echo "" | lynx --accept_all_cookies -cmd_script=$LYNX_CMD_SCRIPT $URL 1>> /dev/null

#Extract the leo_auth_token from the lynx cookie file
cat $LYNX_COOKIES | grep leo_auth_token | cut -f7 > $LEO_AUTH_TOKEN_TEXT

#Replace tokens in cookie with newly extracted values
JSESSION_ID="`cat $JSESSION_TEXT`"
LEO_AUTH_TOKEN="`cat $LEO_AUTH_TOKEN_TEXT`"

#Copy the Cookie template into Cookie.txt so that parameters can be replaced
cp $LINKEDIN_MINER_DIR/Cookie_Template.txt $LINKEDIN_MINER_DIR/Cookie.txt

sed -i "s/%JSESSIONID%/$JSESSION_ID/g" $LINKEDIN_MINER_DIR/Cookie.txt
sed -i "s/%LEO_AUTH_TOKEN%/$LEO_AUTH_TOKEN/g" $LINKEDIN_MINER_DIR/Cookie.txt

#Cleanup
rm index.*

#Run the Mono application
mono $LINKEDIN_MINER_DIR/Application/LinkedInMiner.exe