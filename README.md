## How to work on code for the game

1.  Fork this repository (click the Fork button on Github).

2.  Clone your forked repository to your computer.

        $ git clone git://github.com/your_username/platformer4x4.git

3.  Set up a remote for this repository 

        $ git remote add upstream git://github.com/murphyslaw480/platformer4x4.git
	upstream refers to the main repository you originally forked from. You have another default remote called "origin" that points to your copy of the repo.

4.  Download any changes from your remotes

        $ git fetch --all
	This just retrieves information about commits, it doesn't modify your code.

5.  Pull in any changes from the main master branch

        $ git merge upstream/master

6.  Start a new branch based on the upstream master branch.

        $ git checkout -b newbranchname upstream/master

7.  Commit changes regularly during work

        $git status		#check what files have changed
        $ git add <filename>	#stage a particular file
        $ git add .		#stage all new and modified files
        $ git commit		#commit staged files

8.  Push your commits to your Github repository so others can look at them.

        $ git push origin branchname

9.  Make a pull request on github.com so I can see that you want me to pull your new feature into the master repository.

10. Start working on a new feature (repeat steps 4-10)

### Other useful Git commands

    # View changed or newly added files. Good to get an idea of what you should stage before a commit
    $ git status

    # Stage changed files
    $ git add <file>

    # Commit staged changes
    $ git commit -m "<commit message>"

    # List all branches on your computer
    $ git branch -a

    # Check out a branch
    $ git checkout

    # Make a new branch based on the currently-checked-out branch
    $ git checkout -b <new branch name>

    # Display the commit log
    $ git log

    # Add a named remote repository
    $ git remote add <remote name> <remote url>

    # Download updates from a remote repository
    $ git fetch <remote name>

    # Rebase your current branch on top of another branch
    # This is most likely what you should do if you have made some commits
    # but there are new commits in the master repository that you want to
    # include.
    $ git rebase <branch name>
