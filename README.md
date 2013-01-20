<<<<<<< HEAD
# README for test platformer
This is a test of github for team 4x4s platformer game for CEN3031

# Steps to set up git
1. Install git for Windows
	(go to github.com and follow the instructions)

Follow step 1 on the front page of github.com to set up git.
Then follow step 3 to fork the project, create a clone, set up a remote, and pull in changes.

Try running the project in visual studio to make sure it works for you.
If it does, try making some changes and then pushing back to the remote - 
I will try to merge the changes with the master branch.

You may want to look at Pro Git, a free online book that should explain how to do everything we will need with git.
Especially read chapter 2:
http://git-scm.com/book/en/Git-Basics



#Some Tips
Reverting Changes:
Suppose you make a change and realize you screwed something up.
You can revert a file back to its status in the last commit.
First make sure the file isn't staged:
git reset HEAD 'filename'

Now revert the file to its previous state:
git checkout -- 'filename'

All work on that file since the last commit will be lost

=======
platformer4x4
=============

Test Repository for CEN3031 Project
>>>>>>> d75c83ea999b9e6aa7e47cf441f73aff1f1dedb5
