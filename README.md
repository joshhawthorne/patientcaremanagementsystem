# Patient Care Management System (PCMS)

## MCG Staff Software Engineer Take Home Assessment

### Applicant
- Josh Hawthorne
- joshhawthorne@gmail.com
- 405.413.4995

## Requirements
[MCG Software Engineer Take Home Assessment](Docs/MCG_Software_Engineer_Take_Home_Assessment.pdf)

## RAID Log (Risks, Assumptions, Issues, and Decisions)
[RAID Log (Excel)](Docs/MCG_RAID_Log.xlsx)

## Design

### Approach Used
- [The C4 model for visual software architecture](https://c4model.com/)
- [Lucidchart](https://www.lucidchart.com/) (A free account should suffice to review, the link provided should also allow you to make comments on the diagrams if you would like.)

### System Context Diagram [PCMS]
![System Context Diagram focused on multiple actors, how they interact with the Patient Care Management System, and how that system interacts with other systems across the landscape](Docs/SystemContextDiagram.png)
[Lucidchart](https://lucid.app/lucidchart/6874c0da-aa48-4532-ad36-db17ec0db28e/edit?viewport_loc=-6812%2C-587%2C14038%2C7094%2C0_0&invitationId=inv_5bc533e5-c055-4023-aac0-47b0ec8ebbd1) | [PDF](Docs/SystemContextDiagram.pdf)

### Container Diagram [PCMS]
![Container Diagram focused on a single actor, a physician, and his interaction with the patient care management container](Docs/ContainerDiagram.png)
[Lucidchart](https://lucid.app/lucidchart/6874c0da-aa48-4532-ad36-db17ec0db28e/edit?viewport_loc=-2385%2C-727%2C10367%2C5220%2CvagVQKk2QECb&invitationId=inv_5bc533e5-c055-4023-aac0-47b0ec8ebbd1) | [PDF](Docs/ContainerDiagram.pdf)

### Component Diagram [PCMS - API Application]
![Component Diagram focused on the API Application Component Emphasizes the important of Azure API Management between multiple clients and the Azure Functions](Docs/ComponentDiagram.png)
[Lucidchart](https://lucid.app/lucidchart/6874c0da-aa48-4532-ad36-db17ec0db28e/edit?viewport_loc=-6153%2C74%2C10367%2C5220%2ClojVe~xXzPnl&invitationId=inv_5bc533e5-c055-4023-aac0-47b0ec8ebbd1) | [PDF](Docs/ComponentDiagram.pdf)

### Code
The majority of the work I did was on the [PatientRecordsFunctionApp](API/FunctionApps/PatientRecordsFunctionApp), the corresponding tests in [PatientRecordsFunctionApp.Tests](API/FunctionApps/PatientRecordsFunctionApp.Tests), and [PcmsCore](API/Shared/PcmsApi.Core) Shared Library.

## Summary
This was a great challenge and I really got to think through the health care system in a way I haven't before. Thank you for this opportunity! I thought I would summarize here, what I think went well and what I wish I had more time to add more fit and finish.

### Wins
- Design and up-front documentation
- Using VSCode for the whole exercise. Not sure if I would jump into so many Microsoft Offerings without having fully worked through them in Visual Studio proper first.
- Copilot was very helpful for me getting back up to speed in the sense of hands on keyboard with .Net. Full disclosure, I did get into a few arguments with it. And unless it edits this, I won!
- Use of the RAID Log (linked above). I picked this up a few years ago, and it is nice to be able to offload some of the items as they come up, and then stay focused on the task at hand.
- C4 Models, this is the first time I have used them in a greenfield scenario. It was very helpful to do the exercise of starting top down and diagramming/design to more granularity before starting to code.

### Opportunities 
- I'm a little rusty on .Net, but this helped me see where I'm at and what I can work on.
- I thought I might stand up a developer instance of Okta and use it for Authentication/Authorization. I definitely did not have enough time for that, but I'm familiar with how it handles scopes and claims, so I thought that would be fun to have it translate claims in Azure API Management to parameters on the Azure Functions.
- I wish I had time to get Azure API Management fully configured and working.
- I had planned on doing a ReacJS or Blazor application to connect to AAPIM or the Functions. 
- More unit tests! I ran into some mocking issues with EFCore (why Microsoft still has sealed classes I will never understand).
- Warnings. I am in the camp of treat all warnings like errors. I did not succeed on that effort for this. (13 warnings)