window.saveCurrentRoom = function(userId, json)
{
    DotNet.invokeMethodAsync('Roominator', 'SaveUserRoom', userId, json);
}

window.loadCurrentRoom = function (unityInstance)
{
    let userId = DotNet.invokeMethodAsync('Roominator', 'GetSelectedUserID');
    let json = DotNet.invokeMethodAsync('Roominator', 'GetGeneratedUserRoom');
    unityInstance.SendMessage('SaveManager', 'LoadRoom', userId, json);
}