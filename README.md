# Candidate Management System

Hi! This is a pet project which I am building, primarily to build a better grasp on technologies I use on a day to day basis. 

Keep in mind that this project is still in progress, but you can check out my progress using the following resources:

- Original Mockup on Adobe XD:
    - [Grid View](https://xd.adobe.com/view/29eb7a51-7a5e-490e-8e91-623acbdd03b3-7cb8/grid)
    - [Starting Page](https://xd.adobe.com/view/29eb7a51-7a5e-490e-8e91-623acbdd03b3-7cb8/screen/83113a37-916b-4ea6-8e6e-994d0e53b25e) (I'd suggest starting here)

- Track my progress via GitHub Projects:
    - [General](https://github.com/omkarubale/candidate-management-system/projects/1)

This project has not been deployed yet, but it's in the works. Meanwhile, you can deploy this project locally. Here are the steps:

- Clone the project. You will need Visual Studio (VS), Visual Studio Code (VS Code), SQL Server Management Studio (SSMS), and SQL Server installed.
- This project uses Code First, so setting up the database is simple. 
    - Once you have installed SQL Server, open SSMS and connect to your SQL Server.
    - Open Visual Studio, and then open the *Package Manager Console* and type in `update-database` and press enter. This will create a local database in your SQL Server which you installed earlier.
    - Your database is set up with the name **OU.CMS.Development**.

- Now that your database is set up, to run the API (backend) all you need to do is click the ***IIS Express (Google Chrome)*** button.

- To run the front end:
    - open this directory in VS Code:
 *~repository/OU.CMS.Client/Client-Workspace*
    - in the terminal, type in `npm install`. This will install all the packages which are needed to run the application. This should take 3 to 5 minutes.
    - Once the installation is done, you can type `ng serve -o` to run the application. A new browser tab will open with the application running.
