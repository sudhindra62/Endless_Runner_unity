
using UnityEngine;
using System.Collections.Generic;
using System;

public class FriendManager : Singleton<FriendManager>
{
    public static event Action<List<FriendData>> OnFriendsUpdated;

    private List<FriendData> friends = new List<FriendData>();

    public List<FriendData> Friends => friends;

    // This would be synchronized with a backend service in a real game
    private void Start()
    {
        // Load friends from a local save file or backend
        LoadFriends();
    }

    public void AddFriend(string friendCodeOrUsername)
    {
        // In a real implementation, this would involve a backend API call
        // For now, we'll simulate finding a player and adding them
        Debug.Log($"Attempting to add friend: {friendCodeOrUsername}");
        // Simulate a successful response
        var newFriend = new FriendData
        {
            friendID = $"FRIEND_{UnityEngine.Random.Range(1000, 9999)}",
            username = friendCodeOrUsername, // Use the input as username for simulation
            lastActiveTime = DateTime.UtcNow,
            bestScore = UnityEngine.Random.Range(5000, 25000),
            bestDistance = UnityEngine.Random.Range(1000f, 5000f),
            characterUsed = "DefaultCharacter"
        };

        if (!friends.Exists(f => f.username == newFriend.username))
        {
            friends.Add(newFriend);
            OnFriendsUpdated?.Invoke(friends);
            SaveFriends();
            Debug.Log($"Added {newFriend.username} to friends list.");
        }
        else
        {
            Debug.LogWarning($"{newFriend.username} is already in the friends list.");
        }
    }

    public void RemoveFriend(string friendID)
    {
        friends.RemoveAll(f => f.friendID == friendID);
        OnFriendsUpdated?.Invoke(friends);
        SaveFriends();
    }

    private void LoadFriends()
    {
        // Simulate loading from a persistent source
        // In a real game, this would be SaveManager.Instance.Data, a file, or a backend response
    }

    private void SaveFriends()
    {
        // Simulate saving to a persistent source
    }
}
