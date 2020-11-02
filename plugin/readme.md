
## Environment setup
https://docs.ansible.com/ansible/latest/dev_guide/developing_modules_general.html#developing-modules-general

sudo apt update
sudo apt install build-essential libssl-dev libffi-dev python-dev

git clone https://github.com/ansible/ansible.git
cd ansible
python3 -m venv venv
. venv/bin/activate
pip install -r requirements.txt
. hacking/env-setup

Issues : error: invalid command 'bdist_wheel'
sudo apt-get install gcc libpq-dev -y
sudo apt-get install python-dev  python-pip -y
sudo apt-get install python3-dev python3-pip python3-venv python3-wheel -y
pip3 install wheel

## Graphql client
https://github.com/graphql-python/gql
pip install --pre gql