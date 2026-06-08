# APIs
A collection of REST, SOAP, GraphQL, gRPC &amp; Websocket API's including examples and tests

If you see this you are in the root repository with sub-modules!
- Contact another developer e.g. "Camilla", if something went wrong or it did not succeceed - we all make mistakes, so let's help eachother out soonest <3 And this README needs to be updated... xD

See guides and cheatsheets at SUBMODULES_CHEATS.md or go to the submodules for more details and documentation: https://git-scm.com/book/en/v2/Git-Tools-Submodules

### How clone and get all sub-modules?
1. Open a terminal (CMD, gitBash, etc.)
2. In terminal: Make sure to stand in the folder, where you want to add the repository
(No need for create a new folder)
3. Run git clone like this command in terminal, BUT...
Replace [the repository-link] with the same link you will use for cloning the repository:
``` git clone --recurse-submodules [the repository-link] ```
4. If this work you should have the root repository and all sub-modules downloaded by now (or contact "Camilla") - if not, try the next step below.

### It did not work - what now?
Try clone as you normally would with this command in terminal with git clone....
#### I only got the root repository, but no sub-modules... What do I do now?
Don't worry:
1. Keep the terminal in the same path as you have generated your clone
2. Run following command:
``` git submodule init ``` 
and then:
``` git submodule update ```
3. Your sub-modules should be downloaded by now (or contact "Camilla")

### Docker-compose setup
- If you want to run the APIs with docker-compose, you need to be in the root repository with all sub-modules, and then run this command in terminal:
``` docker-compose up --build ```

#### It can take a while to build the images and start the containers, so be patient BUT if it keeps reloading and does not start, try to stop the docker-compose and run it again with this command in terminal:
``` docker-compose up --build --force-recreate ```

- To stop the docker-compose, run this command in terminal:
``` docker-compose down ```