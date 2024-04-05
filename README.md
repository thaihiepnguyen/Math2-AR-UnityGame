### 1. Project Folder Structure

- Reference: https://unity.com/how-to/organizing-your-project#folder-structure

Assets Folder

```
+----Animations <- to store animations
+----Audio <- to store all audio files
| +---Music
| +---Sound 
+----Scripts <- file entire code
+----Scenes <- to store scenes
+----Fonts <- to contain the fonts used in the game
+----Prefabs <- to reusable GameObjects and add them to a scene to build
```


### 2. Workflow on Github

```mermaid
sequenceDiagram
    participant Main branch
    participant Dev branch
    participant Task demo
    Main branch->>Dev branch: be created from main branch
    Dev branch->>Task demo: be created from dev branch
    Task demo->> Dev branch: After u finish developing, please create a pull request to be reviewed by at least another member in our team
    Dev branch->> Main branch: After verifying, u can merge to dev branch and make sure that there are no bugs.
```
