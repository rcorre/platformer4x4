<<<<<<< HEAD
# README for test platformer
This is a test of github for team 4x4s platformer game for CEN3031

# Steps to set up git

#Add the repository as a remote
git remote add origin git@github.com:murphyslaw480/platformer4x4.git

# Clone the Repository
git clone git://github.com/murphyslaw480/Platformer.git

#Create a Fork
Follow the directions on github.com's front page (Part 3 - Forking a Repository.)

#Working on the project
When adding a new feature, the first step is to get the most recent version of the project.
git fetch
git checkout master
Now that you have the most recent master version, you should run it to make sure it functions and has the most up-to-date features.
If everything works fine, create a new branch to work on your new feature.
git checkout -b my_feature_name
Use a short but descriptive name for my_feature_name. For example, if I were adding a map editor, I might input:
git checkout -b map_editor
Now that you are on a new branch, you are free to work on your new feature without fear of messing up the main copy. You should commit regularly during your work to save changes you make to your branch.
First, stage any files you've added/modified
git add .
git add . will add all new and modified files. You can also specify individual files if you don't want to stage them all by saying:
git add <filename>
now that your files are staged, commit your staged files using
git commit
It will ask you to enter a short message to describe your commit.
You can also use
git commit -m "My message here"
To add the message inline (without going to an editor)
I would recommend commiting every time you test a new part of your feature successfully. For example, if I were making an editor, I might commit once I have some working buttons, commit again when I get tile placement to work, and so on.
Once your feature is complete, you can push your changes to the remote:
git push origin <branch_name>
Where <branch_name> is the name of the branch you are working on (e.g. git push origin map_editor)
Once that is done, you can go to github.com, find your branch, and click on the pull request button to request that I pull your new feature into the master branch.
Once you have added a feature and want to begin work on a new one, remember to make a new branch!

#Reverting Changes
Suppose you make a change and realize you screwed something up.
You can revert a file back to its status in the last commit.
First make sure the file isn't staged:
git reset HEAD <filename>

Now revert the file to its previous state:
git checkout -- <filename>

Note: All work on that file since the last commit will be lost
=======
platformer4x4
=============

Test Repository for CEN3031 Project
>>>>>>> d75c83ea999b9e6aa7e47cf441f73aff1f1dedb5
