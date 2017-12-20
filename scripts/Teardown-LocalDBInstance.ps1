#Requires -RunAsAdministrator

$instanceName = "CompositeQuestionnaire"

sqllocaldb stop $instanceName
sqllocaldb delete $instanceName
