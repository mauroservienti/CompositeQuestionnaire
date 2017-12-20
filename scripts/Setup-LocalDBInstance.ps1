#Requires -RunAsAdministrator

$instanceName = "CompositeQuestionnaire"

sqllocaldb create $instanceName
sqllocaldb share $instanceName $instanceName
sqllocaldb start $instanceName
sqllocaldb info $instanceName
