import { useState } from "react";
import { useAuthContext } from "@/AuthContext";

const Profile = () => {
  const { user, isAuthenticated, isLoading } = useAuthContext();
  const [showModal, setShowModal] = useState(false);
  const [name, setName] = useState(user.name);
  const [email, setEmail] = useState(user.email);

  if (isLoading) {
    return <p>Loading profile...</p>;
  }

  if (!isAuthenticated) {
    return <p>You must be logged in to view this page.</p>;
  }

  const handleUpdate = (e: React.FormEvent) => {
    e.preventDefault();
    // Mock update logic
    console.log("Updated name:", name);
    console.log("Updated email:", email);
    setShowModal(false);
  };

  return (
    <div className="w-[90%] mt-10 p-6 bg-white rounded-2xl shadow-md mx-auto relative">
      <h2 className="text-2xl font-bold mb-6 text-center">User Profile</h2>
      <div className="space-y-4">
        <div>
          <label className="font-medium text-gray-700">User ID:</label>
          <p className="text-gray-900">{user.userId}</p>
        </div>
        <div>
          <label className="font-medium text-gray-700">Name:</label>
          <p className="text-gray-900">{user.name}</p>
        </div>
        <div>
          <label className="font-medium text-gray-700">Email:</label>
          <p className="text-gray-900">{user.email}</p>
        </div>
        <div>
          <label className="font-medium text-gray-700">Role:</label>
          <p className="capitalize text-gray-900">{user.role}</p>
        </div>
      </div>

      <button
        className="mt-6 px-4 py-2 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition"
        onClick={() => setShowModal(true)}
      >
        Update Profile
      </button>

      {/* Modal */}
      {showModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
          <div className="bg-white rounded-2xl p-6 w-full max-w-md shadow-lg">
            <h3 className="text-xl font-semibold mb-4">Update Profile</h3>
            <form onSubmit={handleUpdate} className="space-y-4">
              <div>
                <label className="block text-sm font-medium text-gray-700">
                  Name
                </label>
                <input
                  type="text"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  className="w-full border border-gray-300 rounded-lg px-3 py-2 mt-1 focus:outline-none focus:ring-2 focus:ring-blue-400"
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700">
                  Email
                </label>
                <input
                  type="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  className="w-full border border-gray-300 rounded-lg px-3 py-2 mt-1 focus:outline-none focus:ring-2 focus:ring-blue-400"
                />
              </div>
              <div className="flex justify-end space-x-2">
                <button
                  type="button"
                  onClick={() => setShowModal(false)}
                  className="px-4 py-2 bg-gray-200 rounded-lg hover:bg-gray-300"
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700"
                >
                  Save Changes
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
};

export default Profile;
