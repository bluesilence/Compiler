#include <iostream>
#include <string>
using namespace std;

class Tree {
    public:
	Tree(Tree* l, string k, Tree* r) {
	    left = l;
	    key = k;
	    right = r;
	}

	Tree(string k) : Tree(NULL, k, NULL) {}

	static Tree* insert(string k, Tree* t) {
	    if(NULL == t) {
		return new Tree(k);
	    } else if (k < t->key) {
		return new Tree(insert(k, t->left), t->key, t->right);
	    } else if (k > t->key) {
		return new Tree(t->left, t->key, insert(k, t->right));
	    } else return new Tree(t->left, k, t->right);
	}

	static bool find(string k, Tree* t) {
	    if(NULL == t) {
		return false;
	    } else if(k < t->key) {
		return find(k, t->left);
	    } else if(k > t->key) {
		return find(k, t->right);
	    } else {	//k == t->key
		return true;
	    }
	}

	//PreOrder traverse
	void print() {
	    cout<<key<<' ';
	    if(NULL != left)
		left->print();
	    if(NULL != right)
		right->print();
	}

	~Tree() {
	    cout<<"==>Destructing "<<key<<endl;
	    Release();
	    cout<<"==>Destructed "<<key<<endl;
	}

    private:
	Tree* left;
        Tree* right;
	string key;

	void Release() {
	    cout<<"Release "<<key<<"..."<<endl;

	    if(NULL != left) {
		cout<<"Release left of "<<key<<"..."<<endl;
		delete left;
	    }

	    if(NULL != right) {
		cout<<"Release right of "<<key<<"..."<<endl;
		delete right;
	    }
	}
};

int main() {
    Tree* tree = Tree::insert("t", Tree::insert("l", Tree::insert("root", NULL)));

    cout<<"Print tree: ";
    tree->print();
    cout<<endl;

    cout<<"Find root in tree: "<<Tree::find("root", tree)<<endl;
    cout<<"Find t in tree: "<<Tree::find("t", tree)<<endl;
    cout<<"Find l in tree: "<<Tree::find("l", tree)<<endl;
    cout<<"Find foo in tree: "<<Tree::find("foo", tree)<<endl;

    delete tree;
}
