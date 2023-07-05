# UnitySetupUtility
Welcome to the Unity Project Setup Utility repository! This repository contains a handy utility tool that allows you to set up your Unity projects quickly and easily using a custom editor window. With this tool, you can create main folders for your project, download manifests from GitHub Gist, and effortlessly add or remove useful packages in your project.

![img.png](Screenshots%2Fimg.png)

# Installation
To install the Unity Project Setup Utility via Git link, follow these steps:

1) Open your Unity project.
2) In the Unity editor, navigate to **Window > Package Manager** to open the Package Manager.
3) Click on the **+** button in the top-left corner of the Package Manager window.
4) Choose **Add package from Git URL....**
5) Enter the following Git URL in the provided field: [https://github.com/IronFake/UnitySetupUtility.git#upm](https://github.com/IronFake/UnitySetupUtility.git#upm).
6) Click the **Add** button to begin the installation process.
7) Unity will retrieve the package from the specified Git repository and import it into your project.
8) Once the import is complete, you will be ready to use the utility.

# Getting Started
To get started with the Unity Project Setup Utility, follow these steps:

1) Open your Unity project.
2) In the Unity editor, navigate to **Window > Project Setup Window** to open the utility's editor window.
3) The editor window consists of two tabs: **General** and **Settings**.

### General Tab
The **General** tab provides options for setting up your project:

- **Create Main Folders:** Clicking this button will create the main folders typically used in a Unity project, such as "Scripts," "Scenes," "Art," etc. You can customize these folders by modifying the **Main Folders** section in the **Settings** tab.
- **Download Manifest:** The utility will download the **manifest.json** file from the provided URL and place it in the appropriate location in your project. You can specify the default Gist URL in the **Settings** tab.

### Settings Tab
The **Settings** tab allows you to customize the utility according to your preferences:

- **Main Folders:** Here, you can define the main folder structure for your project. Add, remove, or rename folders as needed. These settings will be used when you click the **Create Main Folders** button in the **General** tab.
- **Gist for Manifest**: Set the default GitHub Gist URL for the manifest.json file. This URL will be used when you click the Download Manifest button in the **General** tab.
- **Gist for List of Packages**: Specify the Gist files associated with different packages you frequently use. Enter the Gist file name and the corresponding GitHub Gist URL. This information will be used when you add or remove packages using the utility.

### Example Gist with List of Packages

To demonstrate the usage of the Unity Setup Utility, we have created an example [Gist that contains a list of packages](https://gist.github.com/IronFake/07ad9070801dbf3aab326e967683ae32) commonly used in Unity projects.

Template of package: **DisplayName "package-name": "url"**

You can use this Gist as a reference or starting point for your own package lists. Simply copy the Gist URL and paste it into the **Gist Files for Packages** section in the **Settings** tab of the Unity Setup Utility. When you open the utility and click on the **Add Package** button, the packages listed in the Gist will be available for selection.

Feel free to customize the Gist by adding or removing packages according to your project requirements.

Customize the settings in the Settings tab according to your project requirements.

# Contributions
Contributions to the Unity Project Setup Utility are welcome! If you find any issues or have suggestions for improvements, please feel free to submit a pull request or open an issue on the repository.

# License
The Unity Project Setup Utility is licensed under the MIT License. You are free to use, modify, and distribute the utility for both commercial and non-commercial purposes. Please refer to the license file for more information.

# Acknowledgments
This utility was developed to streamline the setup process of Unity projects, making it easier for developers to start their projects quickly and efficiently. We would like to acknowledge the open-source community for their valuable contributions and the Unity engine for providing a robust development environment.

Contact
If you have any questions or need further assistance, feel free to contact the repository owner at ironfake98@gmail.com.

Thank you for using the Unity Project Setup Utility! We hope it simplifies your project setup process and enhances your development experience.
