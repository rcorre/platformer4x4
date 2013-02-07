##New: Empty classes, namespaces, and regions
I added four namespaces to the main branch: Model, View, Control, and Data. Read the SDD if you are not sure what
these are for.
In addition, I added some empty classes to each namespace, so the basic classes we will need for the game are already
located in the appropriate namespace. When you want to work on a class, first look if I have already added a definition
for it, if so, just add your code to that definition. If not, create a new class in the appropriate namespace.
(Note: folders in the solution (project) correspond to namespaces. If you add a class to the View folder, Visual Studio
will automatically add it to the View namespace.)

I added region declarations to the empty classes in Model, View, and
Control. The region statements are as follows:

	static	- static variables and methods
	fields	- private/protected instance variables
	properties  -  public instance variables and accessors to private fields
	constructor - constructor(s) for the class
	methods - instance methods (static methods should be in the static region)

These statements don't actually affect the compiled code, but they improve
clarity and organization and allow easy code folding, so you can focus on
just the parts of the code that you need to. Please put your code inside
of the appropriate region.

Note: Many classes may not have any static members or methods- in this
case, you can leave this region blank.
Also, note that the fields region is reserved for private/protected
attributes whereas the properties region should be used for public
attributes or properties that expose private fields (Properties are C#'s
version of Java's get/set accessors)

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
