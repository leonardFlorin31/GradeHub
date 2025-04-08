import { useState } from "react";
import { useAuthContext } from "@/AuthContext";
import { Pencil, UserCircle2 } from "lucide-react"; // optional icon

const Profile = () => {
  const { user, isAuthenticated, isLoading } = useAuthContext();
  const [showModal, setShowModal] = useState(false);
  const [name, setName] = useState(user.name);
  const [email, setEmail] = useState(user.email);

  if (isLoading) {
    return <p className="text-center mt-10">Loading profile...</p>;
  }

  if (!isAuthenticated) {
    return (
      <p className="text-center mt-10">
        You must be logged in to view this page.
      </p>
    );
  }

  const handleUpdate = (e: React.FormEvent) => {
    e.preventDefault();
    console.log("Updated name:", name);
    console.log("Updated email:", email);
    setShowModal(false);
  };

  return (
    <div className="max-w-3xl mx-auto mt-[15%] px-6">
      <div className="bg-white shadow-xl rounded-2xl p-8 relative">
        <div className="flex items-center justify-center mb-6">
          <UserCircle2 className="w-16 h-16 text-blue-500" />
        </div>
        <h2 className="text-3xl font-bold text-center mb-8 text-gray-800">
          User Profile
        </h2>

        <div className="grid grid-cols-1 sm:grid-cols-2 gap-x-12 gap-y-6">
          <ProfileField label="User ID" value={user.userId} />
          <ProfileField label="Name" value={user.name} />
          <ProfileField label="Email" value={user.email} />
          <ProfileField label="Role" value={user.role} />
        </div>

        <div className="mt-10 flex justify-center">
          <button
            onClick={() => setShowModal(true)}
            className="inline-flex items-center gap-2 px-6 py-2 bg-blue-600 text-white rounded-xl hover:bg-blue-700 transition shadow-sm"
          >
            <Pencil className="w-4 h-4" />
            Update Profile
          </button>
        </div>
      </div>

      {/* Modal */}
      {showModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-40">
          <div className="bg-white rounded-xl p-8 w-full max-w-lg shadow-2xl animate-fade-in">
            <h3 className="text-2xl font-semibold mb-6 text-center text-gray-800">
              Edit Profile
            </h3>
            <form onSubmit={handleUpdate} className="space-y-5">
              <div>
                <label className="block text-sm font-medium text-gray-700">
                  Name
                </label>
                <input
                  type="text"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  className="mt-1 w-full border border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
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
                  className="mt-1 w-full border border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-400"
                />
              </div>
              <div className="flex justify-end space-x-3 pt-2">
                <button
                  type="button"
                  onClick={() => setShowModal(false)}
                  className="px-4 py-2 bg-gray-200 text-gray-700 rounded-lg hover:bg-gray-300"
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

// Component for field layout
const ProfileField = ({ label, value }: { label: string; value: string }) => (
  <div>
    <label className="block text-sm font-medium text-gray-600">{label}</label>
    <div className="mt-1 text-gray-900 font-medium bg-gray-50 border rounded-lg px-4 py-2">
      {value}
    </div>
  </div>
);

export default Profile;
