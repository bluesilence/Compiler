#include <iostream>
#include <queue>
#include <functional>
#include <stdlib.h>
using namespace std;

template<class T>
class Median {
	public:
		T getMedian();
		void feed(T num);
	private:
		priority_queue<T> minPart;
		priority_queue<T, vector<T>, greater<T> > maxPart;
};

/*
 * This function keeps the size of minPart always equal to or 1 element greater than that of maxPart
 * Thus, we can always get the median number from the tops of the two parts
 * */
template<class T>
void Median<T>::feed(T num) {
	int sizeOfMin = minPart.size();
	int sizeOfMax = maxPart.size();

	if( sizeOfMin == sizeOfMax ) {
		if( 0 == sizeOfMin ) {
			minPart.push(num);
		} else if( num > maxPart.top() ) {
			minPart.push( maxPart.top() );
			maxPart.pop();
			maxPart.push(num);
		} else {
			minPart.push(num);
		}
	} else { //sizeOfMin == 1 + sizeOfMax
		if( num < minPart.top() ) {
			maxPart.push( minPart.top() );
			minPart.pop();
			minPart.push(num);
		} else {
			maxPart.push(num);
		}
	}
}

template<class T>
T Median<T>::getMedian() {
	int sizeOfMin = minPart.size();
	if( 0 == sizeOfMin ) {
		throw "No element fed.";
	} else {
		int sizeOfMax = maxPart.size();
		if( sizeOfMin == sizeOfMax ) {
			return (minPart.top() + maxPart.top()) / 2;
		} else {
			return minPart.top();
		}
	}
}

int main() {
	Median<int> median;
	srand((unsigned)time(NULL));
	int count = 0;
	do {
		cout<<"Please enter the count of random numbers: ";
		cin>>count;
		if( count <= 0) {
			cout<<"Count should be a positive integer(>0)."<<endl;
		}
	} while (count <= 0);

	for(int i = 1; i <= count; i++) {
		int tmp = rand() % 100;
		cout<<i<<")Feed "<<tmp<<endl;
		median.feed(tmp);
	}

	cout<<"Median number: "<<median.getMedian()<<endl;
}
