def setExists(itemCounts, candidateSet):
    for setCountPair in itemCounts:
        if setCountPair[0] == candidateSet:
            return True

    return False

def incrementCount(itemCounts, candidateSet):
    for setCountPair in itemCounts:
        if setCountPair[0] == candidateSet:
            setCountPair[1] += 1

def countItems(itemSets, candidateSets = set([])):
    itemCounts = [] # List of pairs of [set, count]

    if len(candidateSets) == 0:
        for itemSet in itemSets:
            for item in itemSet:
                candidateSet = set(item)   # Use tuple as key of dict, because list is not hashable
                if setExists(itemCounts, candidateSet):
                    incrementCount(itemCounts, candidateSet)
                else:
                    itemCounts.append([candidateSet, 1])
    else:
        for candidateSet in candidateSets:
            itemCounts.append([candidateSet, 0])

        for candidateSet in candidateSets:
            for itemSet in itemSets:
                if itemSet >= candidateSet: # itemSet includes candidateSet
                    incrementCount(itemCounts, candidateSet)

    return itemCounts

def generateCandidateSets(itemCounts, minsup):
    candidateSets = []
    for itemCount in itemCounts:
        if itemCount[1] >= minsup:
            candidateSets.append(itemCount)   #candidateSet is a list of item-count pair

    return candidateSets

def generateNewItemSets(itemSets, singleCandidates, minsup, candidateSets):
    itemCounts = countItems(itemSets, candidateSets)
    print itemCounts

    newCandidates = generateCandidateSets(itemCounts, minsup)
    print candidateSets

    newItemSets = []
    for candidate in newCandidates:
        for singleItem in singleCandidates:
            if not (candidate[0] >= singleItem[0]):
                #Avoid duplicate join
                combination = candidate[0] | singleItem[0]
                if combination not in newItemSets:
                    newItemSets.append(combination)

    return newItemSets

data = [('A', 'C', 'D'), ('B', 'C', 'E'), ('A', 'B', 'C', 'E'), ('B', 'E')]
minsup = 2

itemSets = []
for item in data:
    itemSets.append(set(item))

singleItems = generateCandidateSets(countItems(data), minsup)
generateNewItemSets(itemSets, singleItems, minsup, [])
