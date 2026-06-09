
# Sub-modules - help and cheats/commands 
- For developers on project

For full documentation: https://git-scm.com/book/en/v2/Git-Tools-Submodules 

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

### How to add a new repo (sub-module)?
Adding a repository as a sub-module:
1. Open a terminal (CMD, gitBash, etc.)
2. In terminal: Make sure to stand in the folder, where you want to add the repository
(No need for create a new folder)
3. Run this command in terminal, BUT...
Replace [the repository/sub-module -link] with the same link you will use for cloning the repository:
``` git submodule add [the repository/sub-module -link] ```
4. IF YOU ARE NOT ON A NEW BRANCH YET... create now a new branch before commiting!
5. Commit the added module with a comment by running this command in terminal, BUT...
Replace [a message about changes] with a revelevant message:
``` git commit -am "[a message about changes]" ```
6. You can now push YOUR NEW BRANCH to git by standing in your new branch with this command, BUT...
Replace [your branch-name] with the name of your actuel new branch - should be an exact copy of name!
``` git push ``` (maybe use ``` git push origin [your branch-name] ``` but be carefull if you work on a forked project!)
7. Tjek by refreshing the Github website and see if the commit has been added to YOUR NEW BRANCH
 - CONTACT another developer e.g. "Camilla", if something went wrong or it did not succeceed - we all make mistakes, so let's help eachother out soonest <3 And this README needs to be updated... xD


### How do I update from a sub-module iiiin this root repository, if there is any updates in the original repository some-where else?
This one it tricky...
To update a sub-module with the newest from branch:
1. Stand in terminal in the root of this repository (the repository collecting all sub-modules)!
2. Write this command in terminal, BUT...
Replace [the repository/sub-module -name] with the exact name from .git folder or fromm .gitmodules file in the root of THIS repository (the repository collecting all sub-modules):
``` git submodule update --remote [the repository/sub-module -name] ```
3. The module should now be updated with the newest from given branch (or contact "Camilla" if it did not work). Repeat the process for other submodules if needed.

### CAMILLA IS INSUCRE ABOUT THIS ONE: How to update the root repository with the newest from sub-modules?
If there are made changes in the sub-module on GitHub and you want to update the root repository with the newest from all the sub-modules, run this command in terminal:
``` git pull --recurse-submodules ```

### How to change or see which branch the sub-module updates comes from?
VERY TRICKY - Not recommended for beginners, but here is how you can do it:
To see or change the branch for the sub-modules: 
1. Go to .gitmodules file in the root of THIS repository (the repository collecting all sub-modules).  
2. (Optional) - You can change it as well but make sure the branch name is an exact copy of orignal branch name from remote repository (the sub-module's origin).

### CAMILLA IS INSUCRE ABOUT THIS ONE: How to see local changes (e.g. before pushing)?
To see your local changes for repository/sub-module run following command, BUT...
Replace [the repository/sub-module -name] with the name of the repository:
``` git diff --cached [the repository/sub-module -name] ```
To see the correct name for the repository in .gitmodules file in the root of THIS repository (the repository collecting all sub-modules). If the name is not an exact copy it might not work.
