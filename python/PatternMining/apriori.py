

def include(itemSet, candidateSet):
    print itemSet
    print candidateSet
    for item in candidateSet:
        if item not in itemSet:
            return False

    return True

def countItems(itemSets, candidateSets = []):
    itemCounts = {}

    if candidateSets == []:
        for itemSet in itemSets:
            for item in itemSet:
                candidateSet = (item,)   # Use tuple as key of dict, because list is not hashable
                if itemCounts.has_key(candidateSet):
                    itemCounts[candidateSet] += 1
                else:
                    itemCounts[candidateSet] = 1
    else:
        for candidateSet in candidateSets:
            itemCounts[candidateSet] = 0

        for candidateSet in candidateSets:
            for itemSet in itemSets:
                if include(itemSet, candidateSet):
                    itemCounts[candidateSet] += 1

    return itemCounts

def generateCandidateSets(itemCounts, minsup):
    candidateSets = {}
    for k,v in itemCounts.items():
        if v >= minsup:
            candidateSets[k] = v   #candidateSet is a list of items

    return candidateSets

def generateNewItemSets(itemSets, singleCandidate, minsup, candidateSets):
    itemCounts = countItems(itemSets, candidateSets)
    print itemCounts

    newCandidates = generateCandidateSets(itemCounts, minsup)
    print candidateSets

    newItemSets = []
    for candidate,count in newCandidates.items():
        for singleItem,singleCount in singleCandidate.items():
            if not include(candidate, singleItem):
                #Avoid duplicate join
                combination = (candidate + singleItem).sort()
                if combination not in newItemSets:
                    newItemSets.append(combination)

    return newItemSets

data = [('A', 'C', 'D'), ('B', 'C', 'E'), ('A', 'B', 'C', 'E'), ('B', 'E')]
minsup = 2

singleItems = generateCandidateSets(countItems(data), minsup)
generateNewItemSets(data, singleItems, minsup, [])
