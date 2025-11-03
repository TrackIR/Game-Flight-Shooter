 # Contributing Guide
How to set up, code, test, review, and release so contributions meet our Definition of Done.

## Code of Conduct
Members are expected to be respectful. Disagreements should be handled through discourse and team decisions. 

## Getting Started
Development setup:
- Download the Unity game engine
- Create a blank Unity 3D project
- Clone the GitHub repo
 - Copy the `Library`, `Logs`, `Packages`, and `UserSettings` from the blank Unity project and place them in the cloned repo’s directory
- In the Unity Hub app click the `Add` then `Add project from disk` and navigate to the cloned repo

## Branching & Workflow
When implementing changes, create a new branch to implement the changes in. Once the changes are complete discuss them with the team and start a pull request. Once the team is happy with the changes the merge can be completed

## Issues & Planning
Planning will take place in team meetings. If a problem arises then it should be discussed with the team. The collective can decide how to best solve it.

## Commit Messages
Commit messages should state what the general purpose of the commit is for. Once an issue is solved by a commit, the issue should be referenced.
> ex: Set up base 3D Unity Project.

## Code Style, Linting & Formatting
We do not currently have code style standards.

## Testing
Tests will be used to check that settings changed in menus are updated correctly. As well as that asteroids are spawning correctly.

## Pull Requests & Reviews
Pull requests should be made when the issue that was trying to be completed is completed. Reviewers should run the game and check that everything is working correctly. Any suggestions or changes should be noted and then decided if action should be taken. At least 2 people should review the changes in a pull request. Once tests are implemented, they should all pass.

## CI/CD
We do not currently have a CI/CD pipeline.

## Security & Secrets
There is no code that needs to be secured or protected.

## Documentation Expectations
When the team decides that a standard needs updating then the appropriate changes will be made.

## Release Process
- Versioning Scheme:  
    - All versions that are not the final build will be formatted with v0.X.X
    - Each playtest build that adds a major feature(s) will increment the first decimal by one
    - Each build that does not add a major feature(s) will increment the second decimal by one
    - i.e., v0.1.1 would be our first ever playtest build
    - What constitutes a major feature will be decided by the team, but will generally include a new section in the changelogs
    - The deliverable version will be v1.0
- Changelog Generation: 
    - Changelogs will be written by the team after each playtest build release.
    - The logs will be split into sections like “Ship,” “Asteroids,” etc.
    - The sections will have bullet points that describe each change made to that section.
    - For new sections, the changelog will say (NEW) next to the name of the section
- Packaging/Publishing Steps: 
    - Since we will not be officially releasing our project, we do not have anything to write about in this section. The packaging and publishing will be entirely handled by the project partners. 
- Rollback Process
    - Since we will not be officially releasing our project, we do not have anything to write about in this section. There will be only one official release, which will be handled by the project partners, and any rollback processes will be handled by them as well. 

## Support & Contact
- Primary contact: Discord project channel
- Expected response: 2-3 hours weekdays
- Partner questions: Microsoft Teams Channel or Project Partner Meeting
    - More urgent communication will occur through email
- TA support: TA meetings (Fridays) or Discord channel
