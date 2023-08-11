using System;
using System.Collections.Generic;
using UnityEngine;

public class LimitedSizeQueue
{
    private Queue<Vector3> queue;
    private int maxSize;
    private int targetIdx = -1;
    private Vector3 lastVector;
    private float lerfArgument;

    public void SetLastVector(Vector3 lastVector)
    {
        this.lastVector = lastVector;
    }

    public LimitedSizeQueue(int maxSize)
    {
        lerfArgument = 0.1f;
        this.queue = new Queue<Vector3>();
        this.maxSize = maxSize;
        lastVector = Vector3.zero;
    }

    public void Enqueue(Vector3 item)
    {
        if (queue.Count >= maxSize)
        {
            queue.Dequeue(); // Remove the oldest item from the front of the Queue
        }

        queue.Enqueue(item);
    }

    public Vector3 Dequeue()
    {
        if (queue.Count == 0)
        {
            throw new InvalidOperationException("Queue is empty.");
        }

        return queue.Dequeue();
    }

    public Vector3 Peek()
    {
        if (queue.Count == 0)
        {
            throw new InvalidOperationException("Queue is empty.");
        }

        return queue.Peek();
    }

    public int Count
    {
        get { return queue.Count; }
    }

    public bool IsEmpty
    {
        get { return queue.Count == 0; }
    }

    public void setArgument(float arg)
    {
        lerfArgument = arg;
    }

    public Vector3 GetFilteredDirectionVector()
    {
        Vector3 filteredDirectionVector;
        if (queue.Count == 0)
        {
            return (Vector3.zero);
        }

        filteredDirectionVector = Vector3.Lerp(lastVector, queue.Peek(), lerfArgument);

        lastVector = filteredDirectionVector;
        
        return (filteredDirectionVector);
    }

    public Vector3 CalculateWeightedAverage()
    {
        if (queue.Count == 0)
        {
            throw new InvalidOperationException("Queue is empty.");
        }

        // Convert the Queue to an array to calculate the average
        Vector3[] queueArray = queue.ToArray();

        // Initialize the sum of x, y, and z components
        float sumX = 0;
        float sumY = 0;
        float sumZ = 0;

        int queueSize = queueArray.Length;
        float totalWeight = 0;
        if (targetIdx == -1)
            targetIdx = FindClosestToMiddleVector();
        else
            targetIdx = FindClosestToLastVector();
        //smallestIdx = FindClosestToMiddleVector();
        
        for (int i = 0; i < queueSize; i++)
        {
            // Assuming Vector3 is Vector3
            Vector3 vectorItem = (Vector3)Convert.ChangeType(queueArray[i], typeof(Vector3));

            // Calculate the weight based on the vector's position in the queue
            //float weight = (i <= 1) ? 0.1f : (i == 2) ? 0.15f : (i == 3) ? 0.25f : 0.40f;
            float weight = 0.15f;
            if (i == targetIdx)
            {
                weight = 1.2f;
                lastVector = (Vector3)Convert.ChangeType(queueArray[i], typeof(Vector3));
            }

            totalWeight += weight;

            // Sum the weighted x, y, and z components
            sumX += vectorItem.x * weight;
            sumY += vectorItem.y * weight;
            sumZ += vectorItem.z * weight;
        }

        // Calculate the weighted average of x, y, and z components
        float averageX = sumX / totalWeight;
        float averageY = sumY / totalWeight;
        float averageZ = sumZ / totalWeight;

        // Return the weighted average Vector3
        return new Vector3(averageX, averageY, averageZ);
    }

    private int FindClosestToLastVector()
    {
        Vector3[] queueArray = queue.ToArray();
        float closestDistance = float.MaxValue;
        int queueSize = queueArray.Length;
        int closestIdx = 0;

        // Loop through all the vectors to find the one closest to the middle vector.
        for (int i = 0; i < queueSize; i++)
        {
            Vector3 vectorItem = (Vector3)Convert.ChangeType(queueArray[i], typeof(Vector3));
            float distance = Vector3.Distance(vectorItem, lastVector);

            if (distance < closestDistance)
            {
                closestIdx = i;
                closestDistance = distance;
            }
        }
        return closestIdx;
    }

    private Vector3 FindMiddleVector()
    {
        Vector3[] queueArray = queue.ToArray();
        int queueSize = queueArray.Length;
        Vector3 sum = new Vector3(0, 0, 0);
        
        for (int i = 0; i < queueSize; i++)
        {            
            Vector3 vectorItem = (Vector3)Convert.ChangeType(queueArray[i], typeof(Vector3));
            sum += vectorItem;
        }
        Vector3 middleVector = sum / queueSize;

        return middleVector;
    }

    private int FindClosestToMiddleVector()
    {
        Vector3 middleVector = FindMiddleVector();        
        Vector3[] queueArray = queue.ToArray();
        float closestDistance = float.MaxValue;
        int queueSize = queueArray.Length;
        int smallestIdx = 0;

        // Loop through all the vectors to find the one closest to the middle vector.
        for (int i = 0; i < queueSize; i++)
        {
            Vector3 vectorItem = (Vector3)Convert.ChangeType(queueArray[i], typeof(Vector3));
            float distance = Vector3.Distance(vectorItem, middleVector);

            if (distance < closestDistance)
            {
                smallestIdx = i;
                closestDistance = distance;                
            }
        }
        return smallestIdx;
    }
}
